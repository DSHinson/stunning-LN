import { InjectionToken } from "@angular/core";
import { ICategoryService } from "./categories.interface";

export const CATEGORY_SERVICE = new InjectionToken<ICategoryService>('CategoryService', {
  providedIn: 'root',
   factory: () => {
    // Default implementation (will be overridden in providers)
    throw new Error('CATEGORY_SERVICE not provided');
  }
});
