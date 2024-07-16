import { apiRequest, ApiResponse } from "../../lib/(cs)fetch";

export interface GetProvidersResponse {
    providers: string[];
}

export const apiGetProviders = async () => {
    return apiRequest<ApiResponse<GetProvidersResponse>>('/api/get_providers');
}
