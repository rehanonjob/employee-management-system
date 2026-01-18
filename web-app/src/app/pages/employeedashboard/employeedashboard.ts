import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ApplyLeave } from '../../components/apply-leave/apply-leave';
import { ILeave } from '../../types/leave';
import { Leave } from '../../services/leave';
import { NgFor, NgClass, CommonModule, NgIf } from '@angular/common';
import { Attendence } from '../../services/attendence';
import { ToastrService } from 'ngx-toastr';
import { ITodayAttendanceStatus } from '../../types/AttendanceStatus';

@Component({
  selector: 'app-employeedashboard',
  imports: [NgFor, NgClass, CommonModule, NgIf],
  templateUrl: './employeedashboard.html',
  styleUrl: './employeedashboard.css',
})
export class Employeedashboard implements OnInit {
  leavelist: ILeave[] = [];

  leaveService = inject(Leave);

  ngOnInit(): void {
    this.loadLeaves();
    this.loadAttendanceStatus();
  }

  constructor(private dialog: MatDialog) {}

  applyLeaveDialog() {
    this.openDialog();
  }

  openDialog() {
    var ref = this.dialog.open(ApplyLeave, {
      width: '50%',
      panelClass: 'm-auto',
    });
    ref.afterClosed().subscribe(() => {
      this.loadLeaves();
    });
  }

  loadLeaves() {
    this.leaveService.GETLEAVESLIST().subscribe({
      next: (resp) => {
        this.leavelist = resp;
        console.log();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  loadAttendanceStatus() {
    this.atendService.GETATTENDTODY().subscribe({
      next: (resp: ITodayAttendanceStatus) => {
        console.log('Attendance status:', resp);
        this.isScannedIn = resp.scannedIn && !resp.scannedOff;
        console.log('isScannedIn set to:', this.isScannedIn);
      },
      error: (err) => {
        console.log('Error loading attendance status:', err);
      },
    });
  }

  cancelLeave(id: any) {
    this.leaveService.CANCELLEAVE(id).subscribe({
      next: (resp) => {
        const leave = this.leavelist.find((l) => l.id === id);
        if (leave) {
          leave.status = 'Cancelled';
        }
      },
      error: (er) => {
        console.log(er);
      },
    });
  }

  atendService = inject(Attendence);
  tot = inject(ToastrService);
  isScannedIn: boolean = false;

  ScanIn() {
    this.atendService.SCANIN().subscribe({
      next: (r) => {
        this.tot.success('Scan-in successful');
        this.isScannedIn = true;
        console.log('ScanIn: isScannedIn set to true');
        this.loadAttendanceStatus(); // Refresh status
      },
      error: (e) => {
        this.tot.error('Scan-in failed');
      },
    });
  }

  ScanOff() {
    this.atendService.SCANOFF().subscribe({
      next: (re) => {
        this.tot.success('Scan-off successful');
        this.isScannedIn = false;
        console.log('ScanOff: isScannedIn set to false');
        this.loadAttendanceStatus(); // Refresh status
      },
      error: (er) => {
        this.tot.error('Scan-off failed');
      },
    });
  }
}
