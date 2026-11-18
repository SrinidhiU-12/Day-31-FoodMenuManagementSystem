import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { CategoryService } from '../../services/category';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './category-form.html',
  styleUrl: './category-form.css'
})
export class CategoryForm implements OnInit {

  id: number | null = null;
  name = '';
  description = '';
  isActive = true;
  message = '';

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.id = Number(id);

      this.categoryService.getById(this.id).subscribe({
        next: category => {
          this.name = category.name;
          this.description = category.description;
          this.isActive = category.isActive;
        },
        error: () => this.message = 'Unable to load category.'
      });
    }
  }

  onSubmit(): void {
    this.message = '';

    const data = {
      name: this.name.trim(),
      description: this.description.trim()
    };

    if (this.id !== null) {
      this.categoryService.update(this.id, {
        ...data,
        isActive: this.isActive
      }).subscribe({
        next: () => this.router.navigate(['/categories']),
        error: err => this.message =
          err.error?.message || 'Unable to update category.'
      });
    } else {
      this.categoryService.create(data).subscribe({
        next: () => this.router.navigate(['/categories']),
        error: err => this.message =
          err.error?.message || 'Unable to create category.'
      });
    }
  }
}