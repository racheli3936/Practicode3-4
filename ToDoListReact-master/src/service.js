import axios from 'axios';
const apiUrl = "http://localhost:5059";

const apiClient=axios.create({
  baseURL:apiUrl
})
apiClient.interceptors.response.use(response=>response,error=>{console.error('axiox error responsse',error.response)
  return Promise.reject(error);
});
export default {
  // פונקציה לקבלת כל המשימות
  getTasks: async () => {
    try {
      const result = await apiClient.get(`/items`);
      return result.data;
    } catch (error) {
      console.error('Error in getTasks:', error.message);
      return [];
    }
  },

  // פונקציה להוספת משימה חדשה
  addTask: async (name) => {
    try {
      const result = await apiClient.post(`/items`, { name ,isComplete:false});
      console.log('addTask', result.data);
      return result.data;
    } catch (error) {
      console.error('Error in addTask:', error.message);
      return {};
    }
  },

  // פונקציה לעדכון סטטוס משימה
  setCompleted: async (id, isComplete) => {
    try {
      const result = await apiClient.put(`/items/${id}?IsComplete=${isComplete}`);
      console.log('setCompleted', { id, isComplete });
      return result.data;
    } catch (error) {
      console.error('Error in setCompleted:', error.message);
      throw error; // חשוב לזרוק את השגיאה כדי שתוכל לתפוס אותה במקום אחר אם יש צורך
    }
  },
  
  // פונקציה למחיקת משימה לפי מזהה
  deleteTask: async (id) => {
    try {
      await apiClient.delete(`/items/${id}`);
      console.log('deleteTask', id);
      return {};
    } catch (error) {
      console.error('Error in deleteTask:', error.message);
      return {};
    }
  },
};
