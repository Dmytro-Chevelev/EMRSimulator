import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <main class="shell">
      <header class="hero">
        <div>
          <h1>EMR Simulator Admin Portal</h1>
          <p>Provider switching, deterministic scenarios, synthetic data, imports, and request logs.</p>
        </div>
      </header>

      <nav class="nav">
        <a routerLink="/providers" routerLinkActive="active">Providers</a>
        <a routerLink="/scenarios" routerLinkActive="active">Scenarios</a>
        <a routerLink="/data" routerLinkActive="active">Data</a>
        <a routerLink="/imports" routerLinkActive="active">Imports</a>
        <a routerLink="/request-logs" routerLinkActive="active">Request Logs</a>
      </nav>

      <section class="content">
        <router-outlet></router-outlet>
      </section>
    </main>
  `,
  styles: [
    `
      .shell {
        max-width: 1100px;
        margin: 0 auto;
        padding: 1.5rem;
      }

      .hero {
        background: linear-gradient(110deg, #285f42, #6ea45a);
        color: #f5f9f2;
        border-radius: 18px;
        padding: 1.2rem 1.4rem;
        box-shadow: 0 18px 30px -20px rgba(20, 45, 26, 0.55);
      }

      .hero p {
        margin-top: 0.5rem;
      }

      .nav {
        margin-top: 1rem;
        display: flex;
        gap: 0.7rem;
        flex-wrap: wrap;
      }

      .nav a {
        text-decoration: none;
        background: var(--surface);
        color: var(--text);
        border: 1px solid #cddabe;
        border-radius: 999px;
        padding: 0.45rem 0.95rem;
        transition: all 0.18s ease;
      }

      .nav a.active,
      .nav a:hover {
        background: var(--accent);
        color: #f7fbf6;
        border-color: var(--accent);
      }

      .content {
        margin-top: 1rem;
        background: var(--surface);
        border: 1px solid #d8e2cb;
        border-radius: 14px;
        padding: 1rem;
        min-height: 58vh;
      }

      @media (max-width: 720px) {
        .shell {
          padding: 1rem;
        }
      }
    `
  ]
})
export class AppComponent {}
