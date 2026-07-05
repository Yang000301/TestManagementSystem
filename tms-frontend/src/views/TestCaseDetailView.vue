<template>
  <div class="testcase-detail-container">
    <div class="header">
      <el-button @click="router.back()">← 返回</el-button>
      <h2>{{ testCase?.title }}</h2>
      <el-button type="primary" @click="showCreateDialog = true">
        新增 Bug
      </el-button>
    </div>

    <el-descriptions :column="2" border style="margin-bottom: 20px">
      <el-descriptions-item label="優先級">{{ testCase?.priority }}</el-descriptions-item>
      <el-descriptions-item label="狀態">{{ testCase?.status }}</el-descriptions-item>
      <el-descriptions-item label="描述" :span="2">{{ testCase?.description }}</el-descriptions-item>
    </el-descriptions>

    <h3>Bug 列表</h3>
    <el-table :data="bugs" style="width: 100%">
      <el-table-column prop="title" label="標題" />
      <el-table-column prop="severity" label="嚴重程度" width="100" />
      <el-table-column prop="status" label="狀態" width="100" />
    </el-table>

    <el-dialog v-model="showCreateDialog" title="新增 Bug" width="400px">
      <el-form :model="newBug">
        <el-form-item label="標題">
          <el-input v-model="newBug.title" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="newBug.description" type="textarea" />
        </el-form-item>
        <el-form-item label="嚴重程度">
          <el-select v-model="newBug.severity">
            <el-option label="Critical" value="Critical" />
            <el-option label="High" value="High" />
            <el-option label="Medium" value="Medium" />
            <el-option label="Low" value="Low" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreateDialog = false">取消</el-button>
        <el-button type="primary" @click="createBug">確認</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios from 'axios'

const router = useRouter()
const route = useRoute()
const testCaseId = route.params.id

const testCase = ref(null)
const bugs = ref([])
const showCreateDialog = ref(false)
const newBug = ref({ title: '', description: '', severity: 'Medium' })

const getToken = () => localStorage.getItem('token')

const fetchTestCase = async () => {
  const response = await axios.get(`https://localhost:7034/api/testcase/${testCaseId}`, {
    headers: { Authorization: `Bearer ${getToken()}` }
  })
  testCase.value = response.data
}

const fetchBugs = async () => {
  const response = await axios.get(`https://localhost:7034/api/bug/testcase/${testCaseId}`, {
    headers: { Authorization: `Bearer ${getToken()}` }
  })
  bugs.value = response.data
}

const createBug = async () => {
  await axios.post('https://localhost:7034/api/bug', {
    ...newBug.value,
    testCaseId: parseInt(testCaseId)
  }, {
    headers: { Authorization: `Bearer ${getToken()}` }
  })
  showCreateDialog.value = false
  newBug.value = { title: '', description: '', severity: 'Medium' }
  fetchBugs()
}

onMounted(() => {
  fetchTestCase()
  fetchBugs()
})
</script>

<style scoped>
.testcase-detail-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
</style>