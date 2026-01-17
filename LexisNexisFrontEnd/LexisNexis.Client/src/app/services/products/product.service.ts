import { inject, Injectable } from '@angular/core';
import { HTTP_SERVICE } from '../http/http.token';
import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { IProductService } from './product.interface';

@Injectable({
  providedIn: 'root'
})

export class ProductService implements IProductService {
  private http = inject(HTTP_SERVICE);
private readonly baseEndpoint = 'products';

 createProduct(product: ProductDto): Observable<ProductDto> {
  return this.http.post<ProductDto>(this.baseEndpoint, product);
  }

  getProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(this.baseEndpoint);
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
