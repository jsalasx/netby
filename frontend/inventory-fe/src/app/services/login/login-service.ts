import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

export interface LoginRequestDto {
  email: string;
  password: string;
}
export interface LoginResponseDto {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;

}

export interface CreateUserRequestDto {
  name: string;
  email: string;
  password: string;
}

export interface CreateUserResponseDto {
  userId: string;
  isSuccess: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private http = inject(HttpClient);

  private baseUrl = 'http://netby.drkapp.com/api/user';



  login(req: LoginRequestDto) {
    return this.http.post<LoginResponseDto>(`${this.baseUrl}/login`, req);
  }

  createUser(req: CreateUserRequestDto) {
    return this.http.post<CreateUserResponseDto>(`${this.baseUrl}`, req);
  }

}
