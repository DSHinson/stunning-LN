import { Signal } from '@angular/core';

export interface IThemeService {
  /**
   * Returns a signal indicating whether dark mode is currently active
   */
  isDarkMode(): Signal<boolean>;

  /**
   * Toggles between dark and light theme
   */
  toggleTheme(): void;

  /**
   * Sets the theme mode explicitly
   * @param isDark - true for dark mode, false for light mode
   */
  setDarkMode(isDark: boolean): void;
}
