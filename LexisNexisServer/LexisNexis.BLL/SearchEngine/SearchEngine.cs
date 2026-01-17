using LexisNexis.Common.Filters;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using System.Linq.Expressions;
using System.Reflection;

namespace LexisNexis.BLL.SearchEngine
{
    public sealed class SearchEngine<T, TKey> : ISearchEngine<T, TKey> where T : EntityBase<TKey>
    {
        private readonly IReadRepository<T, TKey> _repo;
        private readonly List<SearchField> _fields;

        private sealed record SearchField(Func<T, string?> Accessor, double Weight);

        ///<inheritdoc cref="ISearchEngine{T, TKey}"/>
        public SearchEngine(IReadRepository<T, TKey> repository)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));
            // Discover searchable string fields with weights attribute
            _fields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(string))
                .Select(p =>
                {
                    var weight = p.GetCustomAttribute<SearchWeightAttribute>()?.Weight ?? 1.0;
                    Func<T, string?> accessor = item => (string?)p.GetValue(item);
                    return new SearchField(accessor, weight);
                })
                .ToList();

            if (_fields.Count == 0)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} has no searchable string properties.");
            }
        }

        ///<inheritdoc cref="ISearchEngine{T, TKey}.Search(string)"/>
        public async Task<Result<IEnumerable<T>>> Search(string query)
        {
            // Query the repository with the
            Result<IEnumerable<T>> initialResults = await _repo.GetAllAsync();

            if (query is null)
            {
                return initialResults;
            }

            // Handle failure
            if (initialResults is Result<IEnumerable<T>>.Failure failure)
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>(failure.FailureMessage);
            }

            // Extract success data
            Result<IEnumerable<T>>.Success success = (Result<IEnumerable<T>>.Success)initialResults;
            List<T> data = success.Data.ToList();

            // Apply weighted + fuzzy scoring
            IEnumerable<T> scoredItems = data
                .Select(item => new
                {
                    Item = item,
                    Score = ScoreItem(item, query)
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Item);

            return ResultHelpers.ToResult(scoredItems);
        }


        private Expression<Func<T, bool>> BuildPredicate(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return x => true;
            }

            query = query.Trim().ToLowerInvariant();

            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression? combined = null;

            foreach (var field in _fields)
            {
                // x.Property
                var propertyExpr = Expression.Invoke(
                    Expression.Constant(field.Accessor),
                    param
                );

                // x.Property != null
                var notNull = Expression.NotEqual(propertyExpr, Expression.Constant(null, typeof(string)));

                // x.Property.ToLower().Contains(query)
                var toLowerCall = Expression.Call(
                    propertyExpr,
                    typeof(string).GetMethod("ToLower", Type.EmptyTypes)!
                );

                var containsCall = Expression.Call(
                    toLowerCall,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(query)
                );

                var condition = Expression.AndAlso(notNull, containsCall);

                combined = combined == null ? condition : Expression.OrElse(combined, condition);
            }

            if (combined == null)
                combined = Expression.Constant(true); // fallback

            return Expression.Lambda<Func<T, bool>>(combined, param);
        }

        private double ScoreItem(T item, string query)
        {
            double totalScore = 0;

            foreach (var field in _fields)
            {
                var value = field.Accessor(item);
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                double score = ScoreText(value, query);
                totalScore += score * field.Weight;
            }

            return totalScore;
        }

        private static double ScoreText(string text, string query)
        {
            text = text.ToLowerInvariant();

            if (text == query)
                return 1.0;

            if (text.Contains(query))
                return 0.8;

            int distance = LevenshteinDistance(text, query);
            int maxLen = Math.Max(text.Length, query.Length);

            double similarity = 1.0 - (double)distance / maxLen;

            return similarity >= 0.4 ? similarity * 0.6 : 0;
        }

        private static int LevenshteinDistance(string a, string b)
        {
            int[,] d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
            {
                d[i, 0] = i; 
            }
            for (int j = 0; j <= b.Length; j++)
            {
                d[0, j] = j;
            }

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[a.Length, b.Length];
        }


    }

}
