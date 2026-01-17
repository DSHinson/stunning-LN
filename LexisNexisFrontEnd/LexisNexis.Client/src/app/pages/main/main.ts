import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatToolbarModule} from '@angular/material/toolbar';
@Component({
  selector: 'app-main',
  imports: [MatCardModule,MatGridListModule,MatToolbarModule],
  templateUrl: './main.html',
  styleUrl: './main.css',
  standalone: true
})
export class Main {

}
