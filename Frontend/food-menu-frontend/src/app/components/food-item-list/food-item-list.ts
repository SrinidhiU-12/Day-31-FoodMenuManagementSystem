
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FoodItemService } from '../../services/food-item';
import { FoodItem } from '../../models/food-item';

@Component({
  selector: 'app-food-item-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './food-item-list.html',
  styleUrl: './food-item-list.css'
})
export class FoodItemList implements OnInit {
  foodItems: FoodItem[] = [];
  message = '';

  constructor(private foodItemService: FoodItemService) {}

  ngOnInit(): void {
    this.loadFoodItems();
  }

  loadFoodItems(): void {
    this.foodItemService.getAll().subscribe({
      next: data => {
        this.foodItems = data;
        this.message = '';
      },
      error: () => {
        this.message = 'Unable to load food items.';
      }
    });
  }

  deactivate(id: number): void {
    if (!confirm('Are you sure you want to deactivate this food item?')) {
      return;
    }

    this.foodItemService.delete(id).subscribe({
      next: () => this.loadFoodItems(),
      error: () => this.message = 'Unable to deactivate food item.'
    });
  }
}
