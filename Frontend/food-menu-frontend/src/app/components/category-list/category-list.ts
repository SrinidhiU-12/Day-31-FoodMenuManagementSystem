import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../services/category';
import { Category } from '../../models/category';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList implements OnInit {

  categories: Category[] = [];
  message = '';

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: data => {
        this.categories = data;
        this.message = '';
      },
      error: () => {
        this.message = 'Unable to load categories.';
      }
    });
  }

  deactivate(id: number): void {
    if (!confirm('Are you sure you want to deactivate this category?'))
      return;

    this.categoryService.delete(id).subscribe({
      next: () => this.loadCategories(),
      error: () => this.message = 'Unable to deactivate category.'
    });
  }
}