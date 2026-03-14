export interface CategoryDto {
  id: number;
  name: string;
  photoUrl: string;
}

export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  category: CategoryDto;
}
