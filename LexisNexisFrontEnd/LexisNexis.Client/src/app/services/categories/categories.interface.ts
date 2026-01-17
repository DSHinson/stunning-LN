import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';

/**
 * Interface for a Category service.
 * This defines the contract for fetching categories from a backend API.
 * Implementations should be provided via DI using a token.
 */
export interface ICategoryService {
  /**
   * Retrieves all categories from the API.
   * @returns Observable emitting an array of CategoryDto
   */
  getCategories(): Observable<CategoryDto[]>;

  /**
   * Retrieves a single category by ID.
   * @param id The unique ID of the category
   * @returns Observable emitting the CategoryDto, or error if not found
   */
  getCategoryById(id: number): Observable<CategoryDto>;

  /**
   * Retrieves categories structured as a tree (parent-child relationships).
   * Useful for hierarchical displays such as menus or nested lists.
   * @returns Observable emitting an array of CategoryDto representing the root categories with nested children
   */
  getCategoryTree(): Observable<CategoryTreeDto[]>;
}
