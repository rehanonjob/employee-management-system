import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendenceList } from './attendence-list';

describe('AttendenceList', () => {
  let component: AttendenceList;
  let fixture: ComponentFixture<AttendenceList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AttendenceList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttendenceList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
