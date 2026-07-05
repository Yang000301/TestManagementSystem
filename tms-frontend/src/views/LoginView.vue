<template>
  <div class="login-container">
    <el-card class="login-card">
      <h2>Test Management System</h2>
      <el-form :model="form" label-width="80px">
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
          <el-button type="primary" @click="handleLogin" :loading="loading">
            登入
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()
const loading = ref(false)

const form = ref({
  email: '',
  password: ''
})

const handleLogin = async () => {
  loading.value = true
  try {
    const response = await axios.post('https://localhost:7034/api/auth/login', {
      email: form.value.email,
      password: form.value.password
    })

    // 存 Token
    localStorage.setItem('token', response.data.token)

    // 跳到專案列表
    router.push('/projects')
  } catch (error) {
    alert('帳號或密碼錯誤')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f0f2f5;
}

.login-card {
  width: 400px;
}

h2 {
  text-align: center;
  margin-bottom: 20px;
}
</style>