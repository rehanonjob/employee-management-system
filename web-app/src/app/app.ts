import { Component, signal, ViewChild, ViewEncapsulation } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDrawer, MatSidenavModule } from '@angular/material/sidenav';
import { Auth } from './services/auth';
import { MatDialog } from '@angular/material/dialog';
import { Profile } from './pages/profile/profile';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, MatToolbar, MatButtonModule, MatIconModule, MatSidenavModule],
  templateUrl: './app.html',
  encapsulation: ViewEncapsulation.None,
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('web-app');
  @ViewChild('drawer') drawer!: MatDrawer;
  constructor(public authservice: Auth, private dialog: MatDialog) {}
  logout() {
    this.authservice.logout();
  }

  editProfile() {
    this.openDialog();
  }

  openDialog() {
    var ref = this.dialog.open(Profile, {
      // width: '40%',
      panelClass: 'm-auto',
    });
  }
}
