import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginService } from '@app/services/login/login-service';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, CommonModule, ButtonModule, InputTextModule, RouterLink, ToastModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
  providers: [MessageService],
})
export class LoginPage {

  loginForm: FormGroup;
  loginService = inject(LoginService);
  router = inject(Router)
  messageService = inject(MessageService);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onLogin() {
    if (this.loginForm.valid) {
      // Handle login logic here
      this.loginService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          localStorage.setItem('expiresIn', response.expiresIn.toString());
          this.messageService.add({ severity: 'success', summary: 'Login Successful', detail: 'Welcome back!' });
          this.router.navigate(['/transactions']); // Navigate to home or another page after login
          // Redirect or perform other actions on successful login
        }
        , error: (error) => {
          console.error('Login failed', error);
          this.messageService.add({ severity: 'error', summary: 'Login Failed', detail: 'Invalid email or password' });
          // Show error message to the user
        }
      });
    }
  }

}
