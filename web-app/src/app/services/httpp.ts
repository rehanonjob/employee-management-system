import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDepartment } from '../types/department';
import { environment } from '../../environments/environment';
import { IEmployee } from '../types/employee';

@Injectable({
  providedIn: 'root',
})
export class Httpp {
  constructor(private http: HttpClient) {}

  getDepartment() {
    return this.http.get<IDepartment[]>(environment.apiUrl + '/Department');
  }

  postDepartment(name: string) {
    return this.http.post(environment.apiUrl + '/Department', { name: name });
  }

  updateDDepartment(id: number, namee: string) {
    return this.http.put(environment.apiUrl + '/Department/' + id, { name: namee });
  }

  DELETEDepartment(id: number) {
    return this.http.delete(environment.apiUrl + '/Department/' + id);
  }
//--------------------------------------------------------------------------------------------------------------------------------------
  GETEMPLOYEES(filter :any) {
    var params = new HttpParams({fromObject:filter});
  
    return this.http.get<IEmployee[]>(environment.apiempUrl+'?' + params.toString());
  }

  GETEMPLOYEEBYID(id:number)
  {
    return this.http.get<IEmployee>(environment.apiempUrl+ '/' + id)
  }

  ADDEMPLOYEE(emp: IEmployee) {
    return this.http.post(environment.apiempUrl, emp);
  }

  UPDATEEMPLOYEE(id:number,emp:IEmployee){
    return this.http.put(environment.apiempUrl+ '/'+id,emp)
  }

  DELETEEMPLOYEE(id:number){
      return this.http.delete(environment.apiempUrl+'/'+id);
  }
}