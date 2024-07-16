export interface ErrorObject {
    message: string;
    propertyName: string | null;
}

interface ErrorsArray {
    errors?: ErrorObject[] | null;
}

export type ApiResponse<T> = T & ErrorsArray;

export async function apiRequest<T>(url: string, init?: RequestInit): Promise<ApiResponse<T>> {
    const baseUrl = import.meta.env.VITE_API_URL;
    var response = await fetch(baseUrl + url, init);

    try {
        const resultPromise = await response.json() as Promise<ApiResponse<T>>;
        
        return await resultPromise;
    }
    catch {
        return {errors: [{
            message: `Status: ${response.status}. ${response?.statusText}`,
            propertyName: null
        }]} as ApiResponse<T>
    }
}
