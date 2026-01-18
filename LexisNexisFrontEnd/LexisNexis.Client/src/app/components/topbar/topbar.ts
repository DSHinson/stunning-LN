import { Component,inject  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDividerModule } from '@angular/material/divider';
import { THEME_SERVICE } from '../../services/theme/theme.token';
import { IThemeService } from '../../services/theme/theme.interface';

@Component({
  selector: 'app-topbar',
  imports: [CommonModule
    , RouterModule
    , MatToolbarModule
    , MatButtonModule
    , MatIconModule
    , MatMenuModule
    , MatBadgeModule
  , MatDividerModule],
  templateUrl: './topbar.html',
  styleUrl: './topbar.scss',
  standalone: true
})
export class Topbar {
public themeService: IThemeService = inject(THEME_SERVICE);

  toggleTheme() {
    this.themeService.toggleTheme();
  }
}
