import { inject, Injectable } from '@angular/core';
import { HTTP_SERVICE } from '../http/http.token';
import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { IProductService } from './product.interface';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ProductService implements IProductService {
  private http = inject(HTTP_SERVICE);
private readonly baseEndpoint = 'products';

 createProduct(product: ProductDto): Observable<ProductDto> {
  return this.http.post<ProductDto>(this.baseEndpoint, product);
  }

 getProducts(page?: number, pageSize?: number, search?: string, categoryId?: number): Observable<ProductDto[]> {

  // Start with empty HttpParams
  let params = new HttpParams();

  // Only add params that are defined
  if (page != null) {
    params = params.set('page', page.toString());
  }
  if (pageSize != null) {
    params = params.set('pageSize', pageSize.toString());
  }
  if (categoryId != null) {
    params = params.set('category', categoryId.toString());
  }
  if (search) {
    params = params.set('search', search);
  }

  return this.http.get<ProductDto[]>(this.baseEndpoint, { params });
}

  getProductById(id: number): Observable<ProductDto> {
    return this.http.get<ProductDto>(`${this.baseEndpoint}/${id}`);
  }

  updateProduct(id: number, body: Partial<ProductDto>): Observable<ProductDto> {
    return this.http.put<ProductDto>(`${this.baseEndpoint}/${id}`, body);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseEndpoint}/${id}`);
  }
}
