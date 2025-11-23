import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth-service/auth.service';

@Component({
  selector: 'ww-wrapper',
  standalone: true,
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    RouterModule,
  ],
  templateUrl: './wrapper.component.html',
  styleUrl: './wrapper.component.scss',
})
export class WrapperComponent {
  menuList = [
    {
      title: 'Dashboard',
      routerLink: 'dashboard',
    },
    {
      title: 'Task Management',
      routerLink: 'task',
    },
    {
      title: 'Goal Management',
      routerLink: 'goal',
    },
  ];
  selectedMenuIndex!: number;

  constructor(public authService: AuthService, private router: Router) {
    this.menuList.forEach((menu, i) => {
      if (this.router.url.includes(menu.routerLink)) {
        this.selectedMenuIndex = i;
      }
    });
  }

  menuChanged(index: number) {
    this.selectedMenuIndex = index;
  }

  logout() {
    localStorage.clear();
    this.router.navigateByUrl('/login');
  }
}
