import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import { Button, ButtonModule } from 'primeng/button';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Button, ButtonModule, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('inventory-fe');
  private router = inject(Router);
  showNavBar = signal(false);

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      console.log('URL CHANGED:', event.url);
      if (event.url.includes('/login') || event.url.includes('/register')) {
        this.showNavBar.set(false);
      } else {
        this.showNavBar.set(true);
      }
    })
  }

  logout() {
    localStorage.clear();
    window.location.href = '/login';
  }
}
