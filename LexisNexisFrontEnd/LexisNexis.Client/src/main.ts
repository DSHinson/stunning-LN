import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { productReducer } from './app/store/products/product.reducer';
import { ProductEffects } from './app/store/products/product.effects';
import { categoryReducer } from './app/store/categories/category.reducer';
import { CategoryEffects } from './app/store/categories/category.effects';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';

bootstrapApplication(App, {
  ...appConfig,
  providers: [
    ...appConfig.providers,
    provideHttpClient(),
    provideStore({
      products: productReducer,
      categories: categoryReducer,
    }),
    provideEffects(ProductEffects, CategoryEffects),
    provideAnimations(),
        provideToastr({
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      timeOut: 4000
    })
  ]
})
.catch((err) => console.error(err));
