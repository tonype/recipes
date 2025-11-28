export interface Recipe {
  id: string;
  name: string;
  description: string;
  instructions: string;
  notes: string;
  prepTime: number;      // in minutes
  cookTime: number;      // in minutes
  difficulty: number;    // numeric value
  createdAt: string;     // ISO date string
  updatedAt: string;     // ISO date string
}
