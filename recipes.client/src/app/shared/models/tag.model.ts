export interface TagResponse {
  id: string;
  name: string;
  createdAt: string;
  updatedAt: string;
}

export interface TagSelection {
  id?: string; // undefined for new tags
  name: string;
  isNew: boolean;
}

export interface CreateRecipeTagRequest {
  tagId?: string;      // For existing tags (has ID from search)
  tagName?: string;    // For new tags (user-entered name)
}