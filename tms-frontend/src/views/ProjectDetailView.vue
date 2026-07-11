<template>
  <div class="project-detail-container">
    <div class="header">
      <el-button @click="router.push('/projects')">← 返回</el-button>
      <h2>{{ project?.name }}</h2>
      <el-button type="primary" @click="showCreateDialog = true">
        新增測試案例
      </el-button>
    </div>

    <el-table :data="testCases" style="width: 100%">
      <el-table-column prop="title" label="標題" />
      <el-table-column prop="priority" label="優先級" width="100" />
      <el-table-column prop="status" label="狀態" width="100" />
      <el-table-column label="操作" width="150">
        <template #default="scope">
          <el-button size="small" @click="goToTestCase(scope.row.id)">
            查看
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="showCreateDialog" title="新增測試案例" width="400px">
      <el-form :model="newTestCase">
        <el-form-item label="標題">
          <el-input v-model="newTestCase.title" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="newTestCase.description" type="textarea" />
        </el-form-item>
        <el-form-item label="優先級">
          <el-select v-model="newTestCase.priority">
            <el-option label="High" value="High" />
            <el-option label="Medium" value="Medium" />
            <el-option label="Low" value="Low" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreateDialog = false">取消</el-button>
        <el-button type="primary" @click="createTestCase">確認</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '../api'

const router = useRouter()
const route = useRoute()
const projectId = route.params.id

const project = ref(null)
const testCases = ref([])
const showCreateDialog = ref(false)
const newTestCase = ref({ title: '', description: '', priority: 'Medium' })

const fetchProject = async () => {
  const response = await api.get(`/api/project/${projectId}`)
  project.value = response.data
}

const fetchTestCases = async () => {
  const response = await api.get(`/api/testcase/project/${projectId}`)
  testCases.value = response.data
}

const createTestCase = async () => {
  await api.post('/api/testcase', {
    ...newTestCase.value,
    projectId: parseInt(projectId)
  })
  showCreateDialog.value = false
  newTestCase.value = { title: '', description: '', priority: 'Medium' }
  fetchTestCases()
}

const goToTestCase = (id) => {
  router.push(`/testcases/${id}`)
}

onMounted(() => {
  fetchProject()
  fetchTestCases()
})
</script>

<style scoped>
.project-detail-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
</style>