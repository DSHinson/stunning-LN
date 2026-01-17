import { InjectionToken } from "@angular/core";
import { IProductService } from "./product.interface";

export const PRODUCT_SERVICE = new InjectionToken<IProductService>('ProductService', {
  providedIn: 'root',
   factory: () => {
    // Default implementation (will be overridden in providers)
    throw new Error('PRODUCT_SERVICE not provided');
  }
});
