import { Component, inject, OnInit } from '@angular/core';
import { Attendence } from '../../services/attendence';
import { IAttendenceList } from '../../types/IattendenceList';
import { CommonModule } from '@angular/common';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-attendence-list',
  imports: [CommonModule],
  templateUrl: './attendence-list.html',
  styleUrl: './attendence-list.css',
})
export class AttendenceList implements OnInit {
  attendenceService = inject(Attendence);
  authService = inject(Auth);
  route = inject(Router);

  AtaData: IAttendenceList[] = [];

  ngOnInit(): void {
    this.attendenceService.GETALL().subscribe({
      next: (resp) => {
        this.AtaData = resp;
      },
      error: (er) => {
        console.log(er);
      },
    });

    if (!this.authService.isAdmin) {
      alert('Access Denied! You are not authorized to view this page.');
      this.route.navigateByUrl('employee-dashboard');
    }
  }
}
