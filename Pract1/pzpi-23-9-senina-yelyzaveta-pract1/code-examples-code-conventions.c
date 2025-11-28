#include <stdio.h>
 
 #define MAX_SIZE 100
 
 // Функція обчислює суму елементів масиву
 int calculate_sum(int arr[], int size) {
     int sum = 0;
     for (int i = 0; i < size; i++) {
         sum += arr[i];
     }
     return sum;
 }

 int main() {
     int numbers[MAX_SIZE] = { 1, 2, 3, 4, 5 };
     printf("Sum = %d\n", calculate_sum(numbers, 5));
     return 0;
 }
