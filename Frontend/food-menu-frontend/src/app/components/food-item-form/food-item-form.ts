
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { FoodItemService } from '../../services/food-item';
import { CategoryService } from '../../services/category';
import { Category } from '../../models/category';

@Component({
  selector: 'app-food-item-form',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './food-item-form.html',
  styleUrl: './food-item-form.css'
})
export class FoodItemForm implements OnInit {
  id: number | null = null;

  name = '';
  description = '';
  price: number | null = null;
  imageUrl = '';
  categoryId: number | null = null;
  isAvailable = true;

  categories: Category[] = [];
  message = '';

  constructor(
    private foodItemService: FoodItemService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCategories();

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.id = Number(id);
      this.loadFoodItem(this.id);
    }
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: data => {
        this.categories = data.filter(category => category.isActive);
      },
      error: () => {
        this.message = 'Unable to load categories.';
      }
    });
  }

  loadFoodItem(id: number): void {
    this.foodItemService.getById(id).subscribe({
      next: item => {
        this.name = item.name;
        this.description = item.description;
        this.price = item.price;
        this.imageUrl = item.imageUrl;
        this.categoryId = item.categoryId;
        this.isAvailable = item.isAvailable;
      },
      error: () => {
        this.message = 'Unable to load food item.';
      }
    });
  }

  onSubmit(): void {
    this.message = '';

    if (this.price === null || this.categoryId === null) {
      this.message = 'Please enter a price and select a category.';
      return;
    }

    const data = {
      name: this.name.trim(),
      description: this.description.trim(),
      price: this.price,
      imageUrl: this.imageUrl.trim(),
      categoryId: this.categoryId
    };

    if (this.id !== null) {
      this.foodItemService.update(this.id, {
        ...data,
        isAvailable: this.isAvailable
      }).subscribe({
        next: () => this.router.navigate(['/food-items']),
        error: err => {
          this.message = err.error?.message || 'Unable to update food item.';
        }
      });
    } else {
      this.foodItemService.create(data).subscribe({
        next: () => this.router.navigate(['/food-items']),
        error: err => {
          this.message = err.error?.message || 'Unable to create food item.';
        }
      });
    }
  }
}
