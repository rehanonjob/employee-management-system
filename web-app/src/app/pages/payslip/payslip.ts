import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Attendence } from '../../services/attendence';
import { IPaySlips } from '../../types/PaySlips';
import { NgIf } from '@angular/common';
import { CommonModule } from '@angular/common';
import { IAddPaySlip } from '../../types/AddPaySlip';

@Component({
  selector: 'app-payslip',
  imports: [NgIf, CommonModule],
  templateUrl: './payslip.html',
  styleUrl: './payslip.css',
})
export class Payslip implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private httpser: Attendence,
    private router: Router,
  ) {}
  paydata!: IPaySlips;
  errorMessage: string = '';

  ngOnInit() {
    const empId = Number(this.route.snapshot.paramMap.get('id'));

    this.httpser.GETPAYSLIP(empId).subscribe({
      next: (resp) => {
        console.log(resp);
        this.paydata = resp;
      },
      error: (er) => {
        // console.log(er);
        this.errorMessage = er.error?.message || 'Something went wrong';
      },
    });
  }

  AddPaySlip() {
    if (this.paydata) {
      const LoadData: IAddPaySlip = {
        employeeId: this.paydata.employeeId,
        payPeriodStart: this.paydata.startDate,
        payPeriodEnd: this.paydata.endDate,
        paidDays: this.paydata.paidDays,
        totalWorkingDays: this.paydata.attenDays,
        totalEarnings: this.paydata.totalEarnedAmount,
        totalDeductions: this.paydata.totalDeductions,
        netPayable: this.paydata.netPayable,
      };

      this.httpser.ADDSLIP(LoadData).subscribe({
        next: (resp) => {
          alert('Payslip generated successfully');
          this.router.navigateByUrl('employees');
        },
        error: (er) => {
          alert('Payslip already generated');
          this.router.navigateByUrl('employees');
          console.log(er);
        },
      });
    }
  }
}
