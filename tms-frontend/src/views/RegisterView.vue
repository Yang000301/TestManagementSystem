<template>
  <div class="register-container">
    <el-card class="register-card">
      <h2>註冊帳號</h2>
      <el-form :model="form" label-width="80px">
        <el-form-item label="使用者名稱">
          <el-input v-model="form.username" placeholder="請輸入使用者名稱" />
        </el-form-item>
        <el-form-item label="Email">
          <el-input v-model="form.email" placeholder="請輸入 Email" />
        </el-form-item>
        <el-form-item label="密碼">
          <el-input
            v-model="form.password"
            type="password"
            placeholder="請輸入密碼"
            show-password
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleRegister" :loading="loading">
            註冊
          </el-button>
        </el-form-item>
      </el-form>
      <div class="login-link">
        已經有帳號？<router-link to="/login">返回登入</router-link>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api'

const router = useRouter()
const loading = ref(false)

const form = ref({
  username: '',
  email: '',
  password: ''
})

const handleRegister = async () => {
  loading.value = true
  try {
    const response = await api.post('/api/auth/register', form.value)
    localStorage.setItem('token', response.data.token)
    router.push('/projects')
  } catch (error) {
    alert(error.response?.data?.message || '註冊失敗')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f0f2f5;
}

.register-card {
  width: 400px;
}

h2 {
  text-align: center;
  margin-bottom: 20px;
}

.login-link {
  text-align: center;
  margin-top: 10px;
}
</style>