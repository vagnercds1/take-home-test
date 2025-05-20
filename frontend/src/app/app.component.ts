import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { LoanService } from './services/loan.service';
import { Loan } from './models/loan.model'; 
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
 
export class AppComponent {
  title = 'angular-loan-app';
 
  allLoans$ = new Observable<Loan[]>();
  displayedColumns: string[] = ['loanId', 'amount', 'currentBalance', 'status', 'dateInsert', 'dateUpdate', 'applicantId', 'applicantName'];
 
  constructor(private loanService: LoanService) {  
      this.GetAllLoans();
    } 

  GetAllLoans() {
    const credentials = { user: 'john@test.com', password: '1234' };
    this.loanService.getAuthorize(credentials).subscribe(() => {
      this.allLoans$ = this.loanService.getLoans();     
    });
  }
}