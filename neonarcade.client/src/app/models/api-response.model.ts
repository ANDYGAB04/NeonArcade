export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}

export interface ApiError {
  message: string;
  statusCode: number;
  errors?: { [key: string]: string[] };
}

export interface ValidationError {
  field: string;
  message: string;
}
