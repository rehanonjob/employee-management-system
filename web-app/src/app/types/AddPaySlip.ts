export interface IAddPaySlip{
    employeeId:number;
    payPeriodStart:string;
    payPeriodEnd:string;
    paidDays:number;
    totalWorkingDays:number;
    totalEarnings:number;
    totalDeductions:number; 
    netPayable:number;
}