import { InjectionToken } from '@angular/core';
import { IThemeService } from './theme.interface';

export const THEME_SERVICE = new InjectionToken<IThemeService>('ThemeService', {
  providedIn: 'root',
  factory: () => {
    // Default implementation (will be overridden in providers)
    throw new Error('THEME_SERVICE not provided');
  }
});
