import { Injectable, signal, Signal } from '@angular/core';
import { IThemeService } from './theme.interface';

/**
 * Service to manage dark/light theme in the application.
 * Uses CSS classes on <body> to switch between themes.
 * Works with SCSS that defines variables for dark/light mode.
 */
@Injectable({
  providedIn: 'root'
})
export class ThemeService implements IThemeService {
  // Signal representing whether dark mode is active
  private darkMode = signal(true);

  constructor() {
    this.loadThemePreference();
    this.updateThemeClass();
  }

  /** Read-only signal for components to reactively subscribe */
  isDarkMode(): Signal<boolean> {
    return this.darkMode.asReadonly();
  }

  /** Toggle between dark and light mode */
  toggleTheme(): void {
    this.darkMode.update(dark => !dark);
    this.updateThemeClass();
    this.saveThemePreference();
  }

  /** Explicitly set dark mode */
  setDarkMode(isDark: boolean): void {
    this.darkMode.set(isDark);
    this.updateThemeClass();
    this.saveThemePreference();
  }

  /** Apply the appropriate CSS class to <body> */
  private updateThemeClass(): void {
    const body = document.body;
    if (this.darkMode()) {
      body.classList.add('dark-theme');
      body.classList.remove('light-theme');
    } else {
      body.classList.add('light-theme');
      body.classList.remove('dark-theme');
    }
  }

  /** Save preference to localStorage */
  private saveThemePreference(): void {
    localStorage.setItem('theme', this.darkMode() ? 'dark' : 'light');
  }

  /** Load preference from localStorage if it exists */
  private loadThemePreference(): void {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      this.darkMode.set(savedTheme === 'dark');
    }
  }
}
