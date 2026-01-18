import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table',
  imports: [MatTableModule,MatButtonModule],
  templateUrl: './table.html',
  styleUrl: './table.css',
})
export class Table {
[x: string]: any;
@Input() data:any[]=[];
@Input () displayedColumns:any[] =[];
@Output() onEdit = new EventEmitter<any>();
@Output() onDelete = new EventEmitter<any>();
router = inject(Router);

editEmployee(element:any){
this.onEdit.emit(element);
}

deleteEmployee(element:any){
this.onDelete.emit(element);
}

generatePaySlip(empId:number){
  this.router.navigate(['/payslip',empId]);
}

}
