import apiService from "../base-api"
const name = 'Project'
export const Post = (data) => {
    return apiService.post(`${name}/Post`, data)
}
export const Connect = (data) => {
    return apiService.post(`${name}/Connect`, data)
}