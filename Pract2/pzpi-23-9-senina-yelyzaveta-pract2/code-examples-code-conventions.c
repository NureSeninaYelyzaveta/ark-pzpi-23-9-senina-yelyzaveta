// Приклад коду до рефакторингу №1

int temp;
temp = price * quantity;  // обчислення суми товару
total += temp;
temp = price * taxRate;   // обчислення податку
tax += temp;

// Приклад коду після рефакторингу методом Split Temporary Variable (Розділення тимчасової змінної)

int itemTotal = price * quantity;  // сума товару
total += itemTotal;
int itemTax = price * taxRate;     // податок
tax += itemTax;

// Приклад коду до рефакторингу №2

void applyDiscount(double price, double discountRate) {
    discountRate = discountRate / 100;
    price = price - (price * discountRate);
    printf("Final price: %.2f\n", price);
}

// Приклад коду після рефакторингу методом  Remove Assignments to Parameters (Видалення присвоєння параметрам)

void applyDiscount(double price, double discountRate) {
    double discount = discountRate / 100;
    double finalPrice = price - (price * discount);
    printf("Final price: %.2f\n", finalPrice);
}

// Приклад коду до рефакторингу №3

bool found = false;
for (int i = 0; i < n; i++) {
    if (array[i] == target) {
        found = true;
    }
}
if (found) printf("Element found\n");

// Приклад коду після рефакторингу методом Remove Control Flag (Видалення контрольного прапорця)

for (int i = 0; i < n; i++) {
    if (array[i] == target) {
        printf("Element found\n");
        break;  // вихід з циклу одразу після знаходження
    }
}

