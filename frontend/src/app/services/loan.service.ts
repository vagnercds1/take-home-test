import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Loan } from "../models/loan.model";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class LoanService {

  private _apiLoginUrl = environment.apilogin;
  private _apiLoanUrl = environment.apiloan;
  private jwtToken: string | null = null;

  constructor(private _httpClientent: HttpClient) {}

  
  getAuthorize(credentials: { user: string; password: string }): Observable<any> {
    return this._httpClientent.post<any>(this._apiLoginUrl + '/login', credentials)
      .pipe(
        map(response => {
          this.jwtToken = response.token;          
        })
      );
  }

  // Consulta empréstimos usando o JWT no header Authorization
  getLoans(): Observable<Loan[]> {
    const headers = this.jwtToken
      ? new HttpHeaders({ Authorization: `Bearer ${this.jwtToken}` })
      : undefined;

    return this._httpClientent.get<Loan[]>(this._apiLoanUrl + '/loans', { headers });
  }
}