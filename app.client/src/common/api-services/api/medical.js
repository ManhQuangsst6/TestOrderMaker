import apiService from "../base-api"
const name = 'Medical'
export const PostMedical = (data) => {
    return apiService.post(`${name}/Post`, data)
}
export const SetupData = (data) => {
    return apiService.post(`${name}/Setup`, data)
}
