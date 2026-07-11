<template>
  <div class="projects-container">
    <div class="header">
      <h2>專案列表</h2>
      <el-button type="primary" @click="showCreateDialog = true">
        新增專案
      </el-button>
    </div>

    <el-table :data="projects" style="width: 100%">
      <el-table-column prop="name" label="專案名稱" />
      <el-table-column prop="description" label="描述" />
      <el-table-column prop="status" label="狀態" width="100" />
      <el-table-column label="操作" width="150">
        <template #default="scope">
          <el-button size="small" @click="goToProject(scope.row.id)">
            查看
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 新增專案對話框 -->
    <el-dialog v-model="showCreateDialog" title="新增專案" width="400px">
      <el-form :model="newProject">
        <el-form-item label="名稱">
          <el-input v-model="newProject.name" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="newProject.description" type="textarea" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreateDialog = false">取消</el-button>
        <el-button type="primary" @click="createProject">確認</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api'

const router = useRouter()
const projects = ref([])
const showCreateDialog = ref(false)
const newProject = ref({ name: '', description: '' })


const fetchProjects = async () => {
  const response = await api.get('/api/project')
  projects.value = response.data
}

const createProject = async () => {
  await api.post('/api/project', newProject.value)
  showCreateDialog.value = false
  newProject.value = { name: '', description: '' }
  fetchProjects()
}

const goToProject = (id) => {
  router.push(`/projects/${id}`)
}

onMounted(() => {
  fetchProjects()
})
</script>

<style scoped>
.projects-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
</style>