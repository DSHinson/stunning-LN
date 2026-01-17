import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';

export interface IProductService {
  /** Get all products, optionally paginated and filtered by search/category */
  getProducts(
    page?: number,
    pageSize?: number,
    search?: string,
    categoryId?: number
  ): Observable<ProductDto[]>;

  /** Get a single product by its ID */
  getProductById(id: number): Observable<ProductDto>;

  /** Create a new product */
  createProduct(product: ProductDto): Observable<ProductDto>;

  /** Update an existing product */
  updateProduct(id: number, product: ProductDto): Observable<ProductDto>;

  /** Delete a product by its ID */
  deleteProduct(id: number): Observable<void>;
}
