import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

import { THEME_SERVICE } from './services/theme/theme.token';
import { ThemeService } from './services/theme/theme.service';

import { HttpService } from './services/http/http.service';
import { HTTP_SERVICE } from './services/http/http.token';

import { ProductService } from './services/products/product.service';
import { PRODUCT_SERVICE } from './services/products/product.token';

import {CATEGORY_SERVICE} from './services/categories/categories.token';
import { CategoryService} from './services/categories/categories.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    {
      provide: THEME_SERVICE,
      useClass: ThemeService
    },
    {
      provide: HTTP_SERVICE,
      useClass: HttpService
    },
    {
      provide: PRODUCT_SERVICE,
      useClass: ProductService
    },
    {
      provide: CATEGORY_SERVICE,
      useClass: CategoryService
    }
  ]
};
