export interface Loan{  
         loanId?: string;
         amount: string;
         currentBalance: string;
         status : string;
         dateInsert?: string;
         dateUpdate?: string; 
         applicantId: string;
         applicantName: string; 
}