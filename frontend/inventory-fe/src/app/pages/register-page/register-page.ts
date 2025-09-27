import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '@app/services/login/login-service';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, CommonModule, ButtonModule, InputTextModule, ToastModule],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
  providers: [MessageService],
})
export class RegisterPage {
  registerForm: FormGroup;
  loginService = inject(LoginService);
  router = inject(Router)
  messageService = inject(MessageService);

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onRegister() {
    if (this.registerForm.valid) {
      this.loginService.createUser(this.registerForm.value).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.messageService.add({ severity: 'success', summary: 'Registration Successful', detail: 'Welcome!' });
          this.router.navigate(['/login']); // Navigate to login page after registration
        },
        error: (error) => {
          console.error('Registration failed', error);
          this.messageService.add({ severity: 'error', summary: 'Registration Failed', detail: 'Please try again' });
        }
      });
    }
  }
}
