import { apiRequest, ApiResponse } from "../../lib/(cs)fetch";

export interface CalculationResponse {
    result?: string;
}

export const apiCalculate = async (input: string, provider: string) => {
    return apiRequest<ApiResponse<CalculationResponse>>(
        `/api/calculate?query=${encodeURIComponent(input)}&providerKey=${provider}`
    );
}
