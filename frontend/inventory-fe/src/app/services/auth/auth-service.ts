import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  constructor() { }

  getToken(): Promise<string | null> {
    return new Promise((resolve, reject) => {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        reject('No token found');
        return;
      } else {
        resolve(token);
      }
    });
  }
}
