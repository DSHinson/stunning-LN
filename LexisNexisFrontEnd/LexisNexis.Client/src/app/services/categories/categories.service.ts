import { Injectable, inject } from '@angular/core';
import { ICategoryService } from './categories.interface';
import { HttpService } from '../http/http.service';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';
import { Observable } from 'rxjs';

/**
 * Concrete implementation of ICategoryService.
 * Fetches category data from backend endpoints via HttpService.
 */
@Injectable({
  providedIn: 'root'
})
export class CategoryService implements ICategoryService {
  private readonly http = inject(HttpService);
  private readonly baseEndpoint = 'categories';

  /**
   * Get all categories as a flat list
   */
  getCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(this.baseEndpoint);
  }

  /**
   * Get a single category by ID
   * @param id Category ID
   */
  getCategoryById(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.baseEndpoint}/${id}`);
  }

  /**
   * Get the category tree from the backend
   * @returns Observable of hierarchical CategoryTreeDto
   */
  getCategoryTree(): Observable<CategoryTreeDto[]> {
    return this.http.get<CategoryTreeDto[]>(`${this.baseEndpoint}/tree`);
  }
}
