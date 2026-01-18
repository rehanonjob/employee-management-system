import { Component, inject, OnInit } from '@angular/core';
import { Leave } from '../../services/leave';
import { IAdminLeaves } from '../../types/AdminLeaves';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [NgFor, NgIf,CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  leaveService = inject(Leave);
  authService = inject(Auth);
  router = inject(Router);

  adminleavelist: IAdminLeaves[] = [];

  ngOnInit(): void {
    if (!this.authService.isAdmin) {
      this.router.navigate(['/employee-dashboard']);
      return;
    }
    this.leaveService.GETALLLEAVES().subscribe({
      next: (resp) => {
        this.adminleavelist = resp;
      },
      error: (er) => {
        console.log(er);
      },
    });
  }

  redirectdepartment() {
    this.router.navigateByUrl('departments');
  }

  redirectemp(){
    this.router.navigateByUrl('employees');
  }

  redirectaten(){
    this.router.navigateByUrl('attendence-list');
  }

  tot = inject(ToastrService);
  updateStatus(leaveId: number, statusLeave: string) {
    this.leaveService.UPDATELEAVESTATUS({ id: leaveId, status: statusLeave }).subscribe({
      next: (resp) => {
        const leave = this.adminleavelist.find((l) => l.leaveId === leaveId);

        if (leave) {
          leave.status = statusLeave;
        }

        this.leaveService.GETALLLEAVES().subscribe((resp) => {
          this.adminleavelist = resp;
        });

        this.tot.success('Leave request processed successfully');
      },
      error: (er) => {
        console.log(er);
      },
    });
  }
}
